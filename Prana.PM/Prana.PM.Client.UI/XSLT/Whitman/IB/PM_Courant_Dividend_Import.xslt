<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varDividend">
					<xsl:value-of select="COL6"/>
				</xsl:variable>

				<xsl:if test="number($varDividend)">
					
					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'IB'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="normalize-space(COL11)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME = ''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL1!='*'">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Dividend>
							<xsl:choose>
								<xsl:when test="number($varDividend)">
									<xsl:value-of select="$varDividend"/>		
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</Dividend>
						<xsl:variable name="Month" select="substring(COL2,5,2)"/>
						<xsl:variable name="Date" select="substring(COL2,7,2)"/>
						<xsl:variable name="Year" select="substring(COL2,1,4)"/>
						
						<PayoutDate>
							<xsl:value-of select="concat($Month,'/',$Date,'/',$Year)"/>
							
						</PayoutDate>
						<xsl:variable name="Month1" select="substring(COL3,5,2)"/>
						<xsl:variable name="Date1" select="substring(COL3,7,2)"/>
						<xsl:variable name="Year1" select="substring(COL3,1,4)"/>

						<ExDate>
							<xsl:value-of select="concat($Month1,'/',$Date1,'/',$Year1)"/>
						</ExDate>

						<Amount>
							<xsl:choose>
								<xsl:when test="number($varDividend)">
									<xsl:value-of select="$varDividend"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>


						<ActivityType>
							<xsl:choose>

								<xsl:when test="$varDividend &gt; 0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test ="$varDividend &lt; 0">
									<xsl:value-of select ="'DividendExpense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ActivityType>

						<Description>
							
								<xsl:value-of select="COL10"/>

								<!--<xsl:when test="$varDividend &gt; 0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>
								<xsl:when test ="$varDividend &lt; 0">
									<xsl:value-of select ="'Dividend Charged'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>-->

							<!--</xsl:choose>-->
						</Description>

						<CurrencyName>
							<xsl:value-of select="COL8"/>
						</CurrencyName>

						<PBSymbol>
							<xsl:value-of select="$PB_Symbol"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>

				<xsl:variable name="varTax">
					<xsl:value-of select="COL18"/>
				</xsl:variable>

				<xsl:if test="number($varTax) and COL21='TAX'">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'IB'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="normalize-space(COL11)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME = ''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL1!='*'">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Dividend>
							<xsl:choose>
								<xsl:when test="$varTax &gt; 0">
									<xsl:value-of select="$varTax*-1"/>
								</xsl:when>
								<xsl:when test="$varTax &lt; 0">
									<xsl:value-of select="$varTax"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Dividend>

						<PayoutDate>
							<xsl:value-of select="COL3"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="COL2"/>
						</ExDate>

						<Description>
							<xsl:value-of select="'Tax Withheld'"/>
						</Description>

						<CurrencyName>
							<xsl:value-of select="COL8"/>
						</CurrencyName>

						<PBSymbol>
							<xsl:value-of select="$PB_Symbol"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>