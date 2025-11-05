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
						<xsl:with-param name="Number" select="COL18"/>
					</xsl:call-template>
				</xsl:variable>    
        
        <xsl:if test="number($varDividend) and $varDividend!=0 and contains(COL17,'dividend')">    
          
        <PositionMaster>
          <xsl:variable name="PB_Name">
            <xsl:value-of select="'BNPH'"/>
          </xsl:variable>	
          
				  <FundName>
            <xsl:value-of select ="'038QAG8G0 - BNPB'"/>				
				  </FundName>

          <xsl:variable name="varExDate">
            <xsl:value-of select="normalize-space(COL34)"/>
          </xsl:variable>
          <ExDate>
            <xsl:value-of select="$varExDate"/>
          </ExDate>


          <xsl:variable name="varPayoutDate">
            <xsl:value-of select="normalize-space(COL41)"/>
          </xsl:variable>
          <PayoutDate>
            <xsl:value-of select="$varPayoutDate"/>
          </PayoutDate>


          <xsl:variable name="varCurrencyName">
            <xsl:value-of select="normalize-space(COL8)"/>
          </xsl:variable>
          <CurrencyName>
            <xsl:value-of select="$varCurrencyName"/>
          </CurrencyName>


          <Description>
            <xsl:choose>
              <xsl:when test="$varDividend &gt; 0">
                <xsl:value-of select="'Dividend Received'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'Dividend Charged'"/>
              </xsl:otherwise>
            </xsl:choose>
          </Description>



          <xsl:variable name="PB_Symbol" select="COL6"/>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
          </xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
          

          <Amount>
            <xsl:value-of select="$varDividend"/>
          </Amount>
			      

			     <ActivityType>
			     	<xsl:choose>
              <xsl:when test="$varDividend &gt; 0">
			     			<xsl:value-of select="'DividendIncome'"/>
			     		</xsl:when>
			     		<xsl:otherwise>
			     			<xsl:value-of select="'DividendExpenses'"/>
			     		</xsl:otherwise>
			     	</xsl:choose>
			     </ActivityType>  

          
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>