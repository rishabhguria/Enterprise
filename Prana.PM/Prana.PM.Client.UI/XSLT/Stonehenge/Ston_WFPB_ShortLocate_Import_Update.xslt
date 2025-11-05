<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:output method="xml" indent="yes"/>



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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>
  
	<xsl:template match="/">
		<DocumentElement>
		
			<xsl:for-each select ="//PositionMaster">
			
        <xsl:variable name="varQuantity">	
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL2)"/>
          </xsl:call-template>
        </xsl:variable>
     
        <xsl:if test ="number(COL2)">
		
					<PositionMaster>
						
						<Ticker>
							<xsl:value-of select="normalize-space(COL8)"/>
						</Ticker>
						
						<TradeQuantity>
							<xsl:value-of select="COL2"/>
						</TradeQuantity>
						
						<BorrowSharesAvailable>
							<xsl:value-of select="normalize-space(COL3)"/>
						</BorrowSharesAvailable>

                        <xsl:variable name="varBorrowRate">
                          <xsl:value-of select="normalize-space(COL5)"/>
                        </xsl:variable>
						
                        <BorrowRate>
                          <xsl:choose>
                            <xsl:when test="$varBorrowRate='*' or $varBorrowRate='' or $varBorrowRate='GC'">
                              <xsl:value-of select="0"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="$varBorrowRate"/>
                            </xsl:otherwise>
                          </xsl:choose>                         
                        </BorrowRate>		  
				
						<BorrowerId>
							<xsl:value-of select="normalize-space(COL6)"/>
						</BorrowerId>
						
						<StatusSource>
							<xsl:value-of select="normalize-space(COL4)"/>
						</StatusSource>				
						
					</PositionMaster>
          </xsl:if>
			</xsl:for-each>
	
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
