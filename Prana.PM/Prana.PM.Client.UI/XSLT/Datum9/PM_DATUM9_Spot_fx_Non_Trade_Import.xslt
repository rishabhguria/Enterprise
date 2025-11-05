<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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

	<xsl:template name="Date">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="01"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="02"/>
			</xsl:when>
			<xsl:when test="$Month='Mar'">
				<xsl:value-of select="03"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="04"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="05"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="06"/>
			</xsl:when>
			<xsl:when test="$Month='Jul'">
				<xsl:value-of select="07"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="08"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="09"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varDebitCurrency">
					<xsl:value-of select="substring-before(substring-after(normalize-space(COL14),' '),' ')"/>
				</xsl:variable>

				<xsl:variable name="varCreditCurrency">
					<xsl:value-of select="substring-after(substring-after(substring-after(normalize-space(COL14),' '),' '),' ')"/>
				</xsl:variable>

				<xsl:variable name="varNonUSDAmount">
					<xsl:choose>
						<xsl:when test="not(contains($varDebitCurrency,'USD')) or not(contains($varCreditCurrency,'USD'))">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL31"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:variable name="varUSDAmount">
					<xsl:choose>
						<xsl:when test="contains($varDebitCurrency,'USD') or contains($varCreditCurrency,'USD')">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:variable name="Description" select="COL12"/>

				<xsl:if test="number($varNonUSDAmount) and ($varDebitCurrency!='')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPMG'"/>
						</xsl:variable>
						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>

							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</AccountName>

						<xsl:variable name="NonUSDAmount">
							<xsl:choose>
								<xsl:when test="$varNonUSDAmount &gt;0">
									<xsl:value-of select="$varNonUSDAmount"/>
								</xsl:when>
								<xsl:when test="$varNonUSDAmount  &lt;0">
									<xsl:value-of select="$varNonUSDAmount*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<DRCurrencyName>
							<xsl:value-of select="$varDebitCurrency"/>
						</DRCurrencyName>

						<CRCurrencyName>
							<xsl:value-of select="$varCreditCurrency"/>
						</CRCurrencyName>

						<xsl:variable name = "USDAmount" >
							<xsl:choose>
								<xsl:when test="$varUSDAmount &gt; 0">
									<xsl:value-of select="$varUSDAmount"/>
								</xsl:when>
								<xsl:when test="$varUSDAmount &lt; 0">
									<xsl:value-of select="$varUSDAmount*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:choose>
								<xsl:when test="$varDebitCurrency='USD' or $varCreditCurrency='USD'">
									<xsl:value-of select="1"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varCurrency1">
							<xsl:choose>
								<xsl:when test="not(contains($varDebitCurrency,'USD')) or not(contains($varCreditCurrency,'USD'))">
									<xsl:value-of select="COL28"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="FXRate">							
							<xsl:choose>
								<xsl:when test="$varCurrency">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="$varCurrency1">
									<xsl:value-of select="COL28"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<FXRate>
							<xsl:value-of select="$FXRate"/>
						</FXRate>

						<DRFXRate>
							<xsl:choose>
								<xsl:when test="$varCurrency &gt; 0">
									<xsl:value-of select="$varCurrency" />
								</xsl:when>
								<xsl:when test="$varCurrency &lt; 0">
									<xsl:value-of select="$varCurrency * (-1)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
						</DRFXRate>

						<CRFXRate>
							<xsl:choose>
								<xsl:when test="$varCurrency1 &gt; 0">
									<xsl:value-of select="$varCurrency1" />
								</xsl:when>
								<xsl:when test="$varCurrency1 &lt; 0">
									<xsl:value-of select="$varCurrency1 * (-1)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
						</CRFXRate>

						<JournalEntries>
							<xsl:choose>
								<xsl:when test=" $varDebitCurrency ='USD'">
									<xsl:value-of select="concat('Cash', ':' , $USDAmount , '|', 'Cash', ':' , $NonUSDAmount)"/>
								</xsl:when>
								<xsl:when test=" $varCreditCurrency ='USD'">
									<xsl:value-of select="concat('Cash', ':' , $NonUSDAmount , '|', 'Cash', ':' , $USDAmount)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</JournalEntries>

						<!--<xsl:variable name="CurrencyName">
							<xsl:value-of select="normalize-space(COL24)"/>
						</xsl:variable>-->

						<!--<FXConversionMethodOperator>
							<xsl:choose>								
								<xsl:when test="$CurrencyName='AUD' or $CurrencyName='USD' or $CurrencyName='GBP' or $CurrencyName='EUR' or $CurrencyName='NZD'">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when test="$CurrencyName='CAD' or $CurrencyName='DKK' or $CurrencyName='JPY' or $CurrencyName='NOK' or $CurrencyName='SEK'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXConversionMethodOperator>-->
						
						<FXConversionMethodOperator>
							<xsl:choose>
								<xsl:when test="$varCreditCurrency='AUD' or $varCreditCurrency='USD' or $varCreditCurrency='GBP' or $varCreditCurrency='EUR' or $varCreditCurrency='NZD'">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when test="$varCreditCurrency='CAD' or $varCreditCurrency='DKK' or $varCreditCurrency='JPY' or $varCreditCurrency='NOK' or $varCreditCurrency='SEK'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXConversionMethodOperator>

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

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>





