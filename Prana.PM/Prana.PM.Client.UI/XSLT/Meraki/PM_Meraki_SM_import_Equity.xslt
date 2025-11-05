<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

  <xsl:template name="CreateCUSIP">
    <xsl:param name="CUSIP"/>
    <xsl:choose>
      <xsl:when test="$CUSIP!='--'">
        <xsl:choose>
      <xsl:when test="string-length($CUSIP)=1">
        <xsl:value-of select="concat('00000000',$CUSIP)"/> 
      </xsl:when> 
     <xsl:when test="string-length($CUSIP)=2">
        <xsl:value-of select="concat('0000000',$CUSIP)"/> 
      </xsl:when> 
     <xsl:when test="string-length($CUSIP)=3">
        <xsl:value-of select="concat('000000',$CUSIP)"/> 
      </xsl:when> 
     <xsl:when test="string-length($CUSIP)=4">
        <xsl:value-of select="concat('00000',$CUSIP)"/> 
      </xsl:when> 
    <xsl:when test="string-length($CUSIP)=5">
        <xsl:value-of select="concat('0000',$CUSIP)"/> 
      </xsl:when> 
    <xsl:when test="string-length($CUSIP)=6">
        <xsl:value-of select="concat('000',$CUSIP)"/> 
      </xsl:when> 
    <xsl:when test="string-length($CUSIP)=7">
        <xsl:value-of select="concat('00',$CUSIP)"/> 
      </xsl:when> 
    <xsl:when test="string-length($CUSIP)=8">
        <xsl:value-of select="concat('0',$CUSIP)"/> 
      </xsl:when>
    <xsl:otherwise>
    <xsl:value-of select="$CUSIP"/> 
    </xsl:otherwise>
    </xsl:choose>
      
      </xsl:when>
    <xsl:otherwise>
    <xsl:value-of select="''"/> 
    </xsl:otherwise>
    </xsl:choose>
    
  </xsl:template>
  
   <xsl:template name="CreateSEDOL">
    <xsl:param name="SEDOL"/>
    <xsl:choose>
      <xsl:when test="$SEDOL!='--'">
        <xsl:choose>
      <xsl:when test="string-length($SEDOL)=1">
        <xsl:value-of select="concat('00000000',$SEDOL)"/> 
      </xsl:when> 
     <xsl:when test="string-length($SEDOL)=2">
        <xsl:value-of select="concat('0000000',$SEDOL)"/> 
      </xsl:when> 
     <xsl:when test="string-length($SEDOL)=3">
        <xsl:value-of select="concat('000000',$SEDOL)"/> 
      </xsl:when> 
     <xsl:when test="string-length($SEDOL)=4">
        <xsl:value-of select="concat('00000',$SEDOL)"/> 
      </xsl:when> 
    <xsl:when test="string-length($SEDOL)=5">
        <xsl:value-of select="concat('0000',$SEDOL)"/> 
      </xsl:when> 
    <xsl:when test="string-length($SEDOL)=6">
        <xsl:value-of select="concat('000',$SEDOL)"/> 
      </xsl:when> 
    <xsl:otherwise>
       <xsl:value-of select="$SEDOL"/> 
    </xsl:otherwise>
    </xsl:choose>
      
      </xsl:when>
    <xsl:otherwise>
       <xsl:value-of select="$SEDOL"/> 
    </xsl:otherwise>
   </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">
        <xsl:if test="COL12='Equity'">
          <PositionMaster>
            <TickerSymbol>
              <xsl:value-of select="COL16"/>
            </TickerSymbol>

            <UnderLyingSymbol>
              <xsl:value-of select="COL16"/>
            </UnderLyingSymbol>

            <ISINSymbol>
              <xsl:value-of select="COL9"/>
            </ISINSymbol>
            
            <xsl:variable name="varSEDOL">
          <xsl:call-template name="CreateSEDOL">
            <xsl:with-param name="SEDOL" select="COL8"/>
          </xsl:call-template>
            </xsl:variable>
             <SedolSymbol>
              <xsl:value-of select="$varSEDOL"/>
            </SedolSymbol>

            <xsl:variable name="varCUSIP">
          <xsl:call-template name="CreateCUSIP">
            <xsl:with-param name="CUSIP" select="COL7"/>
          </xsl:call-template>
            </xsl:variable>
            <CusipSymbol>
              <xsl:choose>
                <xsl:when test="COL6='--'">
                  <xsl:choose>
                <xsl:when test="$varCUSIP!='--'">
                    <xsl:value-of select="$varCUSIP"/>
                </xsl:when> 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                  </xsl:choose>
                    <xsl:value-of select="''"/>
                </xsl:when> 
              </xsl:choose>              
            </CusipSymbol>

            <BloombergSymbol>
              <xsl:value-of select="COL1"/>
            </BloombergSymbol>

            <AUECID>
              <xsl:value-of select="'1'"/>
            </AUECID>

            <CustomUDA1>
              <xsl:value-of select="COL13"/>
            </CustomUDA1>

            <Sector>
              <xsl:value-of select="COL11"/>
            </Sector>

            <SubSector>
              <xsl:value-of select="COL14"/>
            </SubSector>

            <LongName>
              <xsl:value-of select="COL2"/>
            </LongName>


          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
