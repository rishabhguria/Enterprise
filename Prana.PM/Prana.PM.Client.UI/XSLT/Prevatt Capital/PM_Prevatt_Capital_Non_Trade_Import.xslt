<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL16)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varDescription">
					<xsl:value-of select="normalize-space(COL26)" />
				</xsl:variable>

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varCash">
							<xsl:choose>
								<xsl:when test="$varAmount  &gt; 0">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt; 0">
									<xsl:value-of select="$varAmount*(-1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>
							<xsl:choose>
								<xsl:when  test="COL7 ='Deposit'">
									<xsl:value-of select="concat('Cash',':' , $varCash , '|','CashTransferIn' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7 ='Withdrawal'">
									<xsl:value-of select="concat('CashTransferIn',':' , $varCash , '|','Cash' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7= 'Expenses' and COL26= 'DebitInterest'">
									<xsl:value-of select="concat('IntExpense',':' , $varCash , '|','Cash' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7 = 'Revenue' and COL26 = 'CreditInterest'">
									<xsl:value-of select="concat('Cash',':' , $varCash , '|','Intincome' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7 = 'Expenses' and COL26 = 'MiscExpDirect' ">
									<xsl:value-of select="concat('MISC_EXP',':' , $varCash , '|','Cash' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7= 'Expenses' and COL26= 'MiscRevenueDirect'">
									<xsl:value-of select="concat('MISC_EXP',':' , $varCash , '|','Cash' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7 = 'Revenue' and COL26 = 'MiscRevenueDirect' ">
									<xsl:value-of select="concat('Cash',':' , $varCash , '|','MISC_INC' ,':', $varCash)"/>
								</xsl:when>
								<xsl:when  test="COL7= 'SpotFX'">
									<xsl:value-of select="concat('Cash',':' , $varCash , '|','Cash' ,':', $varCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<Description>
							<xsl:choose>
								<xsl:when  test="COL7 ='Deposit'">
									<xsl:value-of select="normalize-space(COL7)"/>
								</xsl:when>
								<xsl:when  test="COL7 ='Withdrawal'">
									<xsl:value-of select="normalize-space(COL7)"/>
								</xsl:when>
								<xsl:when  test="COL7= 'Expenses' and COL26= 'DebitInterest'">
									<xsl:value-of select="normalize-space(COL26)"/>
								</xsl:when>
								<xsl:when  test="COL7 = 'Revenue' and COL26 = 'CreditInterest'">
									<xsl:value-of select="normalize-space(COL26)"/>
								</xsl:when>
								<xsl:when  test="COL7 = 'Expenses' and COL26 = 'MiscExpDirect' ">
									<xsl:value-of select="normalize-space(COL26)"/>
								</xsl:when>
								<xsl:when  test="COL7= 'Expenses' and COL26= 'MiscRevenueDirect'">
									<xsl:value-of select="normalize-space(COL26)"/>
								</xsl:when>
								<xsl:when  test="COL7 = 'Revenue' and COL26 = 'MiscRevenueDirect' ">
									<xsl:value-of select="normalize-space(COL26)"/>
								</xsl:when>
								<xsl:when  test="COL7= 'SpotFX'">
									<xsl:value-of select="'SPOT FX'"/>
								</xsl:when>
							</xsl:choose>
						</Description>

						<FXRate>
							<xsl:value-of select="normalize-space(COL19)"/>
						</FXRate>

						<xsl:variable name="PB_CURRENCY_NAME">
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="$PB_CURRENCY_NAME"/>
						</CurrencyName>

						<xsl:variable name="varTradeDate">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select="$varTradeDate"/>
						</Date>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>