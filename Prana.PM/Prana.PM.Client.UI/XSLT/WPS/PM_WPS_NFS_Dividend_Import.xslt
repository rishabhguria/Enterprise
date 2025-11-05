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
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name="varDividend">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14 div 100"/>
					</xsl:call-template>
				</xsl:variable>    
          <xsl:if test="number($varDividend) and COL7='DIV'">    
        <PositionMaster>
          <xsl:variable name="PB_Name">
            <xsl:value-of select="''"/>
          </xsl:variable>			
             
        
          <xsl:variable name = "PB_FUND_NAME" >
            <xsl:value-of select="COL3"/>
          </xsl:variable>
          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <xsl:variable name = "varAccount" >
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable> 
          
						<AccountName>
                  <xsl:value-of select ="$varAccount"/>				
						</AccountName>


          <xsl:variable name="PB_Symbol" select="COL57"/>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
          </xsl:variable>


          <xsl:variable name="Symbol" select="normalize-space(COL57)"/>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

          <xsl:variable name = "varAmount" >
            <xsl:choose>
              <xsl:when test="$varDividend &gt; 0">
                <xsl:value-of select="$varDividend"/>
              </xsl:when>
              <xsl:when test="$varDividend &lt; 0">
                <xsl:value-of select="$varDividend*(-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          

						<Amount>
              <xsl:value-of select="$varAmount"/>
						</Amount>



            <xsl:variable name="varDayName">
              <xsl:value-of select="substring(COL10,7,2)"/>
            </xsl:variable>
            
            <xsl:variable name="varMonthName">
              <xsl:value-of select="substring(COL10,5,2)"/>
            </xsl:variable>
            
            <xsl:variable name="varYearName">
              <xsl:value-of select="substring(COL10,1,4)"/>
            </xsl:variable>
            
            
            
            <xsl:variable name="varDateName">
              <xsl:value-of select="concat($varMonthName,'-',$varDayName,'-',$varYearName)"/>
            </xsl:variable>


            <RecordDate>
              <xsl:value-of select="$varDateName"/>
            </RecordDate>

            <PBSymbol>
              <xsl:value-of select ="$PB_Symbol"/>
            </PBSymbol>
          
          
						<Description>
							<xsl:value-of select="COL19"/>
						</Description>  
          
						<ActivityType>
              <xsl:value-of select="COL7"/>
						</ActivityType>  

          
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>