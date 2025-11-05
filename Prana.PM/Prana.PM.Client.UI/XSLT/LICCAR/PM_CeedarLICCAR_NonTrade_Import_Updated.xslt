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
						<xsl:with-param name="Number" select="normalize-space(COL21)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varDescription">
					<xsl:value-of select="normalize-space(COL12)" />
				</xsl:variable>

				<xsl:if test="number($varAmount)  and ($varDescription = 'WFS Credit Interest New' or 
				  $varDescription = 'WFS Debit Interest New' or $varDescription = 'WFS Security Lending Revenue New' or 
				  $varDescription = 'WFS Ticket Charge New' or  $varDescription = 'Deposit New' or $varDescription = 'Withdrawal New' )">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
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

						<xsl:variable name="PB_CURRENCY_NAME">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<CurrencyName>
							<xsl:value-of select="$PB_CURRENCY_NAME"/>
						</CurrencyName>				
						
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
								<xsl:when test="$varAmount &gt; 0 and (contains($varDescription,'Deposit') )">
									<xsl:value-of select="concat('Cash',':', $varCash , '|', 'CASH_DEP', ':' , $varCash)"/>
								</xsl:when>
								<xsl:when  test="$varAmount &lt; 0 and (contains($varDescription,'Withdrawal'))">
									<xsl:value-of select="concat('CASH_WDL',':' , $varCash , '|' ,'Cash',':', $varCash)"/>
								</xsl:when>
								<xsl:when test="$varAmount &gt; 0 and (contains($varDescription,'WFS Credit Interest New') or (contains($varDescription,'WFS Security Lending Revenue New')))">
									<xsl:value-of select="concat('Cash',':' , $varCash , '|', 'MISC_INC', ':' , $varCash)"/>
								</xsl:when>
								<xsl:when  test="$varAmount &lt; 0 and (contains($varDescription,'WFS Debit Interest New') or (contains($varDescription,'WFS Ticket Charge New')))">
									<xsl:value-of select="concat('MISC_EXP',':' , $varCash , '|','Cash' ,':', $varCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<Description>
							<xsl:value-of select="$varDescription"/>
						</Description>

						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="$varFXRate &gt; 0">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>
								<xsl:when test="$varFXRate &lt; 0">
									<xsl:value-of select="$varFXRate * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name="varTradeDate">
							<xsl:value-of select="normalize-space(COL10)"/>
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