<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
        <xsl:if test="number(translate(COL14,'$','')) and (COL1='69642321'or COL1='1PT00052'or COL1='69191389'or COL1='59792710' or COL1='10111644' or COL1='71240526' or COL1='71240527' or COL1='71240528' or COL1='78726923' or COL1='80025901' or COL1='82394500' or COL1='80025908')">
					<!--TABLE-->
					<PositionMaster>				
											
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="COL4"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSec']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>
            <FundName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>
            
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
