<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:my1="put-your-namespace-uri-here"
  xmlns:my2="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>



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
						<xsl:with-param name="Number" select="normalize-space(COL13)"/>
					</xsl:call-template>
				</xsl:variable>		

				<xsl:if test="number($varAmount) and COL3 = 'DIV'">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="'BTIG'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME" select="COL22"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
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

						<xsl:variable name="varSymbol" select="normalize-space(COL5)"/>
						<xsl:variable name="varCurrency" select="normalize-space(COL7)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="string-length($varSymbol) = 7 and $varCurrency != 'USD'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol !=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<SEDOL>
							<xsl:choose>
								
								<xsl:when test="string-length($varSymbol) = 7 and $varCurrency != 'USD'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:when test="$varSymbol !='' ">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>
						
						<Amount>
							<xsl:choose>
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>

						
						
						<ExDate>
							<xsl:value-of select="normalize-space(substring-after(COL22,':'))" />
						</ExDate>

						<RecordDate>
							<xsl:value-of select="''" />
						</RecordDate>

						
						<PayoutDate>
							<xsl:value-of select="COL2" />
						</PayoutDate>
			
						<CurrencyName>
							<xsl:value-of select="substring-before(COL7,'_')"/>
						</CurrencyName>

						<ActivityType>
							<xsl:choose>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="'DividendExpense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ActivityType>
						
						<Description>
							<xsl:choose>
								<xsl:when test="$varAmount &gt;0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>	
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="'Dividend Charged'"/>
								</xsl:when>	
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

				<xsl:variable name = "PB_CURRENCY_NAME" >
				  <xsl:value-of select="COL7"/>
			  </xsl:variable>
				<xsl:variable name="varAmount1">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL18)"/>
					</xsl:call-template>
				</xsl:variable>	
				<xsl:variable name="varAmount2">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL13)"/>
					</xsl:call-template>
				</xsl:variable>	
				
				<xsl:variable name="varFXRATE">
				  <xsl:choose>
					  <xsl:when test ="$PB_CURRENCY_NAME !='USD'">
						  <xsl:value-of select="$varAmount1 div $varAmount2"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'1'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
				<FXRate>
				  <xsl:value-of select="$varFXRATE"/>
			  </FXRate>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>