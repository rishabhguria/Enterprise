<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
        <!--<xsl:if test="number(translate(COL14,'$','')) and (COL1='64860812' or COL1='41954500')">-->
			<xsl:if test="number(translate(COL14,'$','')) and (COL1='64860812' or COL1='41954500' or COL1='11520755')">
					<!--TABLE-->
					<PositionMaster>				
											
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="COL4"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSec']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>						
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:variable name="PRANA_FUND_ID">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GSec']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
						</xsl:variable>

						<AccountID>
							<xsl:value-of select="$PRANA_FUND_ID"/>
						</AccountID>

						<!--Change by ashish -->

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME=''">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="Amount">
							<xsl:value-of select="number(translate(COL14,'$',''))"/>
						</xsl:variable>
						<Amount>
							<xsl:value-of select="$Amount"/>
						</Amount>
						<PayoutDate>
							<xsl:value-of select="COL8"/>
						</PayoutDate>
						<ExDate>
							<xsl:value-of select="COL6"/>
						</ExDate>

						<RecordDate>
							<xsl:value-of select="COL7"/>
						</RecordDate>

						<DeclarationDate>
							<xsl:value-of select="COL5"/>
						</DeclarationDate>

						<ActivityType>
							<xsl:choose>
								<xsl:when test="$Amount &gt; 0">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>
								<xsl:when test="$Amount &lt; 0">
									<xsl:value-of select="'DividendExpense'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ActivityType>

						<Description>
							<xsl:choose>
								<xsl:when test="$Amount &gt; 0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>
								<xsl:when test="$Amount &lt; 0">
									<xsl:value-of select="'Dividend Charged'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>				

					</PositionMaster>
					
				</xsl:if >
			</xsl:for-each>			
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
