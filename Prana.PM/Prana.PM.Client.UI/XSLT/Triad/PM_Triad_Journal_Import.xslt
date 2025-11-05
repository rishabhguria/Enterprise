<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>



	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varCash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14 div 100"/>
					</xsl:call-template>
				</xsl:variable>
				
				

				<xsl:variable name="varTest" select="COL7"/>
					<xsl:variable name="varResult">
						<xsl:choose>
						<xsl:when test="$varTest='INT'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varTest='WTT'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varTest='FAD'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varTest='FEE'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'False'" />
						</xsl:otherwise>
						</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varAccount" select="COL3"/>
				<xsl:variable name="varAccResult">
					<xsl:choose>
						<xsl:when test="$varAccount='400168'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400014'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400015'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400016'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400017'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400022'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400023'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400024'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400025'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400026'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400027'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400028'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400029'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400030'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400040'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400043'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='400045'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:when test="$varAccount='930002'">
							<xsl:value-of select="'True'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'False'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				

				<xsl:if test="number($varCash) and COL119='*' and $varResult='True' and $varAccResult='True'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fidelity'"/>
						</xsl:variable>



						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varDayName">
							<xsl:value-of select="substring(COL9,7,2)"/>
						</xsl:variable>

						<xsl:variable name="varMonthName">
							<xsl:value-of select="substring(COL9,5,2)"/>
						</xsl:variable>

						<xsl:variable name="varYearName">
							<xsl:value-of select="substring(COL9,1,4)"/>
						</xsl:variable>



						<xsl:variable name="varDateName">
							<xsl:value-of select="concat($varMonthName,'/',$varDayName,'/',$varYearName)"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select="$varDateName"/>
						</Date>




						<xsl:variable name ="Description" select="normalize-space(COL19)"/>
						<Description>
							<xsl:choose>
								<xsl:when test="$Description!=''">
									<xsl:value-of select="$Description" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''" />
								</xsl:otherwise>
							</xsl:choose>
						</Description>





						<xsl:variable name="varCurrencyName" select="COL119"/>
						<CurrencyName>
							<xsl:choose>
								<xsl:when test="$varCurrencyName='*'">
									<xsl:value-of select="'USD'" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''" />
								</xsl:otherwise>
							</xsl:choose>
						</CurrencyName>



						<xsl:variable name = "varAmount" >
							<xsl:choose>
								<xsl:when test="$varCash &gt; 0">
									<xsl:value-of select="$varCash"/>
								</xsl:when>
								<xsl:when test="$varCash &lt; 0">
									<xsl:value-of select="$varCash*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varTransactionType" select="normalize-space(COL7)"/>
						<xsl:variable name ="varDesc" select="normalize-space(COL19)"/>
						<xsl:variable name ="varSign" select="normalize-space(COL15)"/>


						<xsl:variable name="PRANA_ACRONYM_NAME">
							<xsl:choose>
								<xsl:when test="$varTransactionType='INT'">
									<xsl:choose>
										<xsl:when test="($varDesc='FULLY PAID' or $varDesc='CREDIT INTEREST' or $varDesc='MARGIN INTEREST' or $varDesc='USB CAPITAL IX') and $varSign='-'">
											<xsl:value-of select="'InterestIncome'"/>
										</xsl:when>
										<xsl:when test="($varDesc='FULLY PAID' or $varDesc='CREDIT INTEREST' or $varDesc='MARGIN INTEREST' or $varDesc='USB CAPITAL IX') and $varSign='+'">
											<xsl:value-of select="'Interest Expense'"/>
										</xsl:when>
										<xsl:when test="$varDesc='SHORT SALE BORROWING' and $varSign='-'">
											<xsl:value-of select="'MISC_INC'"/>
										</xsl:when>
										<xsl:when test="$varDesc='SHORT SALE BORROWING' and $varSign='+'">
											<xsl:value-of select="'MISC_EXP'"/>
										</xsl:when>
										<xsl:when test="$varDesc='SHORT SALE REBATE' and $varSign='-'">
											<xsl:value-of select="'MISC_INC'"/>
										</xsl:when>
										<xsl:when test="$varDesc='SHORT SALE REBATE' and $varSign='+'">
											<xsl:value-of select="'MISC_EXP'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$varTransactionType='WTT'">
									<xsl:choose>
										<xsl:when test="contains($varDesc,'WD') and $varSign='-'">
											<xsl:value-of select="'CASH WDL'"/>
										</xsl:when>
										<xsl:when test="contains($varDesc,'WD') and $varSign='+'">
											<xsl:value-of select="'CASH DEP'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$varTransactionType='FAD'">
									<xsl:choose>
										<xsl:when test="$varDesc='PB FEE Dec Equities' and $varSign='-'">
											<xsl:value-of select="'MISC_INC'"/>
										</xsl:when>
										<xsl:when test="$varDesc='PB FEE Dec Equities' and $varSign='+'">
											<xsl:value-of select="'MISC_EXP'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$varTransactionType='FEE'">
									<xsl:choose>
										<xsl:when test="$varDesc='FOREIGN EXCHANGE FEE' and $varSign='-'">
											<xsl:value-of select="'MISC_INC'"/>
										</xsl:when>
										<xsl:when test="$varDesc='FOREIGN EXCHANGE FEE' and $varSign='+'">
											<xsl:value-of select="'MISC_EXP'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$varSign='-'">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $varAmount, '|Cash:',$varAmount)"/>
								</xsl:when>
								<xsl:when test="$varSign='+'">
									<xsl:value-of select="concat( 'Cash:',$varAmount,'|', $PRANA_ACRONYM_NAME,':' , $varAmount)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>