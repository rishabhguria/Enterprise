<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
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

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
	
      <xsl:for-each select="ThirdPartyFlatFileDetail[Side='Sell short']">
      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>
        <!--  system inetrnal use -->
        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

        <BorrowBroker>
          <xsl:value-of select="translate(TradeAttribute1,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
        </BorrowBroker>

        <TradeDate>
          <xsl:value-of select="TradeDate"/>
        </TradeDate>

        <Symbol>
          <xsl:value-of select="Symbol"/>
        </Symbol>

        <SecurityName>
          <xsl:value-of select="FullSecurityName"/>
        </SecurityName>

		
		 <xsl:variable name="varTradeAttribute3">
         <xsl:call-template name="Translate">
           <xsl:with-param name="Number" select="TradeAttribute3"/>
         </xsl:call-template>
       </xsl:variable>
        <LocatedQty>
		<xsl:choose>
		 <xsl:when test="$varTradeAttribute3=''">
                <xsl:value-of select="''"/>
              </xsl:when>
			  <xsl:when test="$varTradeAttribute3='*'">
                <xsl:value-of select="''"/>
              </xsl:when>
			  <xsl:when test="$varTradeAttribute3=' '">
                <xsl:value-of select="''"/>
              </xsl:when>
			  <xsl:when test="$varTradeAttribute3='NaN'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varTradeAttribute3"/>
              </xsl:otherwise>
            </xsl:choose>
       
        </LocatedQty>

        <LocatedID>
          <xsl:value-of select="TradeAttribute2"/>
        </LocatedID>

        <Rate>
		<xsl:choose>
              <xsl:when test="TradeAttribute4=''">
                <xsl:value-of select="'GC'"/>
              </xsl:when>
			  <xsl:when test="TradeAttribute4='*'">
                <xsl:value-of select="'GC'"/>
              </xsl:when>
			  <xsl:when test="TradeAttribute4=' '">
                <xsl:value-of select="'GC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="TradeAttribute4"/>
              </xsl:otherwise>
            </xsl:choose>
         
        </Rate>

      </ThirdPartyFlatFileDetail>
	   </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>